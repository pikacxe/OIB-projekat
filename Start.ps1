# Define the paths to the exe files
$fim_exe = "OIBIS\FileIntegritiMonitoringProject\bin\Debug\FileIntegrityMonitoringProject.exe"
$ips_exe = "OIBIS\IntrusionPreventionSystemProject\bin\Debug\IntrusionPreventionSystemProject.exe"
$fm_exe = "OIBIS\FileManagerProject\bin\Debug\FileManagerProject.exe"
$client_exe = "OIBIS\ManagerClients\bin\Debug\ManagerClients.exe"


Write-Host "Starting up service, please wait...." -F Yellow

# Function to run an executable as a specific user
function Run-ExeAsUser($exePath) {
    # Get the directory containing the executable
    $exeDir = Split-Path -Path $exePath -Parent

    # Get executable name
    $exeFile = Split-Path -Path $exePath -Leaf

    Write-Host "Starting $exeFile service..." -F Yellow

    # Start service with user prompt
    Start-Process -FilePath $exeFile -WorkingDirectory $exeDir -Verb RunAsUser 
    Write-Host "$exeFile service started!" -F DarkGreen
}

# Run Client
Run-ExeAsUser $client_exe

#Run FM service
Run-ExeAsUser $fm_exe

# Run IPS service
Run-ExeAsUser $ips_exe

# Run FIM service
Run-ExeAsUser $fim_exe

Write-Host "Services started successfully..." -F White -B DarkGreen