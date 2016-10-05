:: /install        - installs the service
:: /uninstall      - uninstalls the service
:: /console        - starts the program as console application
:: [/ServiceName=] - service name
:: [/Account=]     - possible user accounts are: LocalService, LocalSystem(default), NetworkService, User
:: [/UserName=]    - user name
:: [/Password=]    - password
:: /help           - this help
:: /?              - this help



:: Install the service...

"%~dp0/Evidence.OutlookIntegration.WindowsService.exe" /install /ServiceName=EvidenceOutlookIntegrationService
@echo off
set /p X=Press Enter to exit...