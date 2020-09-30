# dotnetcore-windows-service


### Run Command
Run powershell as admin
> sc.exe create SamTestService_002 binpath= "D:\Laboratory\WorkerServicePoC\WorkerServicePoC\bin\Release\netcoreapp3.1\publish\WorkerServicePoC.exe"
sc.exe delete SamTestService_002

### Guides
* https://anthonygiretti.com/2020/01/02/building-a-windows-service-with-worker-services-and-net-core-3-1-part-1-introduction/
* https://www.codeproject.com/Articles/5263939/Build-a-Windows-Worker-Service-Using-NET-Core-3-1