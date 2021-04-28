#基于 `microsoft/aspnetcore:2.0` 来构建我们的镜像
FROM microsoft/aspnetcore:2.0
#拷贝项目publish文件夹中的所有文件到 docker容器中的publish文件夹中  
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "Ator.Site.dll"]