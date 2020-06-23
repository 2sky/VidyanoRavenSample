# VidyanoRavenSample

This project uses Vidyano (https://www.vidyano.com) as an application framework with RavenDB (https://ravendb.net/) as database server for persisted storage running as a ASP.NET Core 3.1 application on docker.

## Getting started

** Requirements **

* Clone the repository locally
* Visual Studio (https://visualstudio.microsoft.com/downloads/)
* Docker Desktop (https://www.docker.com/products/docker-desktop)
* RavenDB running locally (https://ravendb.net/download) or modify the appsettings.json to point to your RavenDB server

Open Visual Studio and run the application, at startup the database will be created automatically and populated with the RavenDB sample data.
On first login use admin/admin to log in after which you'll be asked to change the password.