To use this Ocelot with consule need to download consul.exe from https://www.consul.io/downloads & put this in root folder parelel to this file

1.Do open -  cmd to current folder & run below command
consul agent -dev
2.Run all projects
3.Try to access serviceAPI1 & serviceAPI2 using API gateway baseurl
http://localhost:15203/api/Test/APIGateway
http://localhost:15203/ServiceAPI1/api/Test/GetTestData1
http://localhost:15203/ServiceAPI2/api/Test/GetTestData2