OpenCover.Console.exe -register:user -target:"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\MSTest.exe" -targetargs:" /testcontainer:F:\coding\TomatoDoer\TomatoDoer.Tests.Unit\bin\Release\TomatoDoer.Tests.Unit.dll /noresults" -filter:+[TomatoDoer]* -output:opencovertests.xml

"..\ReportGenerator.1.2.4.0\ReportGenerator.exe" opencovertests.xml reports
pause