::start .\"NUnit 2.6.3\bin\nunit-console.exe" /runlist:.\assetlist.txt .\Run\Asset.dll /work:.\Report\Asset /out:assetLog.txt
::start .\"NUnit 2.6.3\bin\nunit-console.exe" /runlist:.\changelist.txt .\Run\Change.dll /work:.\Report\Change /out:changeLog.txt
::start .\"NUnit 2.6.3\bin\nunit-console.exe" /runlist:.\configurationlist.txt .\Run\Configuration.dll /work:.\Report\Configuration /out:configurationLog.txt
start .\"NUnit 2.6.3\bin\nunit-console.exe" /runlist:.\incidentlist.txt .\Run\Incident.dll /work:.\Report\Incident /out:incidentLog.txt
::start .\"NUnit 2.6.3\bin\nunit-console.exe" /runlist:.\knowledgelist.txt .\Run\Knowledge.dll /work:.\Report\Knowledge /out:knowledgeLog.txt
::start .\"NUnit 2.6.3\bin\nunit-console.exe" /runlist:.\portallist.txt .\Run\Portal.dll /work:.\Report\Portal /out:portalLog.txt
::start .\"NUnit 2.6.3\bin\nunit-console.exe" /runlist:.\problemlist.txt .\Run\Problem.dll /work:.\Report\Problem /out:problemLog.txt
::start .\"NUnit 2.6.3\bin\nunit-console.exe" /runlist:.\workorderlist.txt .\Run\WorkOrder.dll /work:.\Report\WorkOrder /out:workorderLog.txt
