$ErrorActionPreference = 'Continue'

$logDir = if ($env:CLAUDE_PROJECT_DIR) { $env:CLAUDE_PROJECT_DIR } else { $PSScriptRoot | Split-Path | Split-Path }
$logPath = Join-Path $logDir '.claude\hooks\csharpier-format.log'
$stamp = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'

try {
    $raw = [Console]::In.ReadToEnd()
    Add-Content -Path $logPath -Value "[$stamp] invoked. pwd=$($PWD.Path) projectEnv=$env:CLAUDE_PROJECT_DIR stdinBytes=$($raw.Length)"

    if (-not $raw) {
        Add-Content -Path $logPath -Value "[$stamp]   skip: empty stdin"
        exit 0
    }

    $payload = $raw | ConvertFrom-Json
    $path = $payload.tool_input.file_path
    Add-Content -Path $logPath -Value "[$stamp]   file_path=$path"

    if (-not $path) {
        Add-Content -Path $logPath -Value "[$stamp]   skip: no file_path"
        exit 0
    }
    if (-not $path.EndsWith('.cs')) {
        Add-Content -Path $logPath -Value "[$stamp]   skip: not a .cs file"
        exit 0
    }

    Push-Location $env:CLAUDE_PROJECT_DIR
    try {
        $output = & dotnet csharpier format $path 2>&1
        Add-Content -Path $logPath -Value "[$stamp]   csharpier exit=$LASTEXITCODE output=$output"
    }
    finally {
        Pop-Location
    }
}
catch {
    Add-Content -Path $logPath -Value "[$stamp]   ERROR: $($_.Exception.Message)"
    exit 0
}
