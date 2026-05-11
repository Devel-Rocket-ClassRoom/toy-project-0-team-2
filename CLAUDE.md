# CLAUDE.md

이 저장소에서 Claude Code 작업 시 참고할 가이드입니다.

## 프로젝트

Unity 6 (`6000.3.11f1`, URP) 기반 클래시 로얄 스타일 1v1 게임. 8장 덱 중 4장을 핸드로 받아 카드를 배치하고, 유닛이 다리를 건너 타워를 공격해 먼저 크라운 3개를 얻으면 승리. 게임 코드는 `Assets/Scripts/`, 카드 SO는 `Assets/ScriptableObjects/<CardName>/`.

## 작업 환경

CLI 빌드/테스트는 없음. Unity Editor에서 Play로 반복. 빌드 씬은 [MainTitleScene.unity](Assets/Scenes/Main%20Scene/MainTitleScene.unity)와 `MainBattleScene.unity` 두 개. `Main Scene AJW/`, `Main SceneKsj/`는 개인 작업용이라 빌드에 포함 안 됨.

- C# 포맷터 (CSharpier, `dotnet-tools.json`에 고정):
  - `dotnet tool restore` (클론 후 1회)
  - `dotnet csharpier .` 또는 `dotnet csharpier <path>`
- IDE 솔루션: `toy-project-0-team-2.slnx` (VS의 *Managed Game* 워크로드 필요, `.vsconfig` 참고). `Assembly-CSharp.csproj`는 Unity가 생성하므로 직접 수정 금지.
- `Library/`, `Logs/`, `Temp/`, `UserSettings/`, `*.csproj`, `*.slnx`은 git ignore. 커밋된 건 IDE 편의용이며 Unity가 덮어쓰도록 둘 것.
- 자동화된 테스트 타깃 없음 (Unity Test Framework는 manifest에 있으나 테스트 어셈블리 미정의).

## 아키텍처

### 엔티티 라이프사이클 (단일 진실 공급원)

[EntityManager.cs](Assets/Scripts/Game/Rule/EntityManager.cs)는 **static** 레지스트리. 모든 유닛/타워는 `redTeamEntities` / `blueTeamEntities`에 등록되고, 크라운 타워는 `redTeamCrownTower` / `blueTeamCrownTower`에 미러링. 타깃팅·승리 조건·AI 모두 이 리스트를 참조. King Tower(`entityId == -1`) 제거 시 `RemoveEntities`가 같은 팀의 나머지 크라운 타워에 `Dead()`를 캐스케이드 — 킹 파괴로 매치 종료. 공개 이벤트: `onCrounTowerDestroy`, `onEntitiesChanged`.

**static이라 씬 전환·에디터 도메인 리로드로 클리어되지 않음.** 잔존 엔티티 버그는 여기서부터 디버깅.

### 카드 데이터 모델 (ScriptableObject, [Assets/Scripts/Game/Card/SO/](Assets/Scripts/Game/Card/SO/))

- `CardData` — 플레이어가 배치하는 단위. 엘릭서 코스트, `arrangmentCompletTime` (스웜 스폰 간격), `CardDataStructure[]` (스폰 항목별 `EntityData` + `positionAdjustment`).
- `EntityData` (abstract) → `UnitData`, `TowerData`, `SpellData`. 각각 `AttackData` / `DefenseData` / `SpecialData` 서브 SO 조합. `DefenseData.entityType`은 `[Flags]` enum (`Ground | Aerial | Tower | CrownTower`)으로 스폰 레이어 및 `attackFilter` 매칭에 사용.
- `Team` ([IDamageable.cs](Assets/Scripts/Game/Card/IDamageable.cs)): `RedTeam`, `BlueTeam`, `Neutrality`.

### 컨트롤러 ([Assets/Scripts/Game/Card/](Assets/Scripts/Game/Card/))

상속: `RootController` → `EntityController` (abstract, 팀/체력/카드 데이터) → `UnitController` / `TowerController` / `AttackEntityController`. 동작은 `Component/`의 형제 MonoBehaviour로 조합: `TargetFinder`, `EntityMover` (NavMesh), `EntityAttacker`, `SoundPlayer`. `UnitController`는 `EntityState` (`Idle`, `LookingForTarget`, `Attack`, `Sprint` …) FSM을 `Update`에서 실행.

`EntityMover`는 첫 `Awake`에서 `CardArrangementManager.Instance.Mid` / `RedArena[1]`을 읽어 아레나 지오메트리를 **static** 필드(`VerticalMidLine`, `HorizontalMidLine`, `ArenaTowerLine`, `RoadLine`)에 캐싱. AI·레인 락·스펠 배치 모두 이 static을 참조 — 아레나 피벗 이동이나 `CardArrangementManager` 초기화 우회 시 이동 로직이 깨짐.

### 스폰 파이프라인

플레이어 → `CardManager.UsedCard(index, screenPoint)`에서 적절한 레이어 마스크로 레이캐스트 (스펠은 `GroundLayerMask`, 유닛은 팀 마스크 — 각 팀은 자기 진영에만 배치 가능) → `CardArrangementManager.Arrangement(card, team, point)` 코루틴:

1. `team == BlueTeam`이고 크라운 타워가 아니면 `AI.PlayerArrangementCard`로 전달해 AI가 반응하게 함.
2. 각 `CardDataStructure`마다 `unitPrefab` / `towerPrefab` / `spellPrefab` (서브클래스 기준)로 인스턴스화, `Init(...)` 호출, `EntityManager.AddEntities` 등록, `CardManager.CreateHealthBar` / `CreateTowerHealthBar`로 월드 스페이스 체력바 생성.
3. `arrangmentCompletTime / count` 간격으로 대기 (스웜 시차).

`CardArrangementManager.Start`에서 크라운 타워 4개 (`KingTower` + 사이드별 `ArenaTower` 2개)도 초기화.

### AI ([AI.cs](Assets/Scripts/Game/AI/AI.cs))

탐색 기반이 아닌 반응형. 핸드를 `EntityTypeDetail` 버킷 (BigUnit / MiddleUnit / WiniUnit / Tower / Magic / Recycle)으로 엘릭서 코스트와 `cardDatas.Length` 휴리스틱으로 분류 → `SelectCard`가 상대 버킷에 대한 하드코딩된 선호 테이블로 카운터 선택. `ChooseReactMethod`가 반환하는 `ReactMethod` (`Rear`, `Mid`, `DefenseArenaTower`, `ArenaTowerShiled`, `AfterAcrossBridge`, `Magic`)는 `EntityMover`의 static 아레나 라인 기준 고정 오프셋에 매핑. 유휴 엘릭서: `elixir > 9.5`면 플레이어가 마지막에 낸 고엘릭서 카드와 반대 레인에 가장 싼 비마법 카드를 배치. **AI 튜닝은 거의 항상 `ArrangementCard`의 오프셋과 `ClassifyCard`의 버킷 임계값을 함께 수정** — 둘을 일관되게 유지할 것.

### 덱과 씬 흐름

`DeckContainer`는 `DontDestroyOnLoad` 싱글톤. 타이틀→배틀 씬 전환에도 살아남고 `Deck` (`CardData[8]`) 노출. `HandManager`가 `Awake`에서 읽어 셔플 후 4장을 `handCards`로 드로우. 타이틀 씬 UI (`UiManager` → `UiMainWindow`/`UiCardWindow`)에서 덱을 채우고, `SceneChanger.ChangeScene`이 8칸 전부 채워졌는지 검증 후 `SceneManager.LoadScene` 호출.

### 승리 조건

`Gameendmanager`가 `Update`마다 `BattleUI.RedCrownCount` / `BlueCrownCount`와 `timerManager.battleTime`을 폴링해 `onGameEnd(losingTeam)` 발생. `EntityController.Init`에서 모든 엔티티가 이 이벤트를 구독하므로 게임 종료 시 패배 팀 유닛이 모두 자체 `Dead()` 처리.

## 알아둘 컨벤션

- **한글 주석과 Debug.Log 문자열이 일반적** — 주변 코드 편집 시 자동 번역 금지.
- 개발자별 평행 씬/에셋 폴더 다수 (`KsjPrefabs`, `Test_Ajw`, `Main Scene AJW` …). 개인 작업 공간이므로 정식 `Assets/Scenes/Main Scene/` 빌드 씬을 우선 편집.
- 여러 시스템이 **static** 상태 사용 (`EntityManager`, `EntityMover` 아레나 라인, `CardArrangementManager` / `DeckContainer` / `UiManager` 싱글톤). 새 상태 추가 시 도메인 리로드나 `CardArrangementManager.Awake`를 우회하는 테스트 씬에서 초기화 누락 주의.
- `Gameendmanager.onGameEnd`는 평범한 `static Action<Team>`이며 **해제되지 않음**. 새 구독자 추가 시 `OnDestroy`에 `-=` 짝을 맞춰야 에디터 플레이 세션 간 참조 누수 방지.
