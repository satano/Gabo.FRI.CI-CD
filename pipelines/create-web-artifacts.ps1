param (
    [Parameter(Mandatory = $true)][string]$ApiUrl,
    [Parameter(Mandatory = $true)][string]$InputFolder,
    [Parameter(Mandatory = $true)][string]$OutputFolder
)

$jsFile = "$InputFolder/app.js"
(Get-Content $jsFile)
    | ForEach-Object { $_ -replace "const apiUrl = "".*?""", "const apiUrl = ""$ApiUrl""" }
    | Set-Content $jsFile

if (-not (Test-Path $OutputFolder)) {
    New-Item -Path $OutputFolder -ItemType Directory
}

$outputFile = "$OutputFolder/web.zip"
if (Test-Path $outputFile) {
    Remove-Item $outputFile
}

Compress-Archive -Path "$InputFolder/*" -DestinationPath $outputFile
