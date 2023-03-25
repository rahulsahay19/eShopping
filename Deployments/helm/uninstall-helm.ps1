Param(
    [parameter(Mandatory=$false)][string]$appName="eshopping"    
    )

Write-Host "Starting Helm Uninstallation" -ForegroundColor Green

# Uninstallation Process

$listOfComponents=$(helm ls --filter $appName -q)    
if ([string]::IsNullOrEmpty($listOfComponents)) {
    Write-Host "No previous installation found!!!" -ForegroundColor Green
}else{
    Write-Host "Found previous releases!!!" -ForegroundColor Green
    Write-Host "Removing previous helm releases..." -ForegroundColor Green
    helm uninstall $listOfComponents

    Write-Host "Removed previous releases" -ForegroundColor Green
}        


