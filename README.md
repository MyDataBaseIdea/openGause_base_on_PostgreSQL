# openGause_base_on_PostgreSQL
openGause_base_on_PostgreSQL
## openGauss:
### 必備環境
#### Docker Desktop
##### https://www.docker.com/products/docker-desktop/
#### Azure Data Studio
##### https://docs.microsoft.com/zh-tw/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15
### Docker 指令集
#### docker pull enmotech/opengauss:2.1.0
#### docker run --name opengauss --privileged=true -d -p 5432:5432 -e GS_PASSWORD=Trq@7251 enmotech/opengauss:2.1.0
#### docker exec -it opengauss bash
### Nuget 元件
#### NpgSQL
