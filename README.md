# Deploying .NET Machine Learning Models With ML.NET, ASP.NET Core, Docker and Azure Container Instances

This is a sample solution of how machine learning models built with [ML.NET](https://www.microsoft.com/net/learn/apps/machine-learning-and-ai/ml-dotnet) framework can be exposed to clients via an [ASP.NET Core](https://www.microsoft.com/net/learn/apps/web) Web API that has been packaged into a [Docker](https://www.docker.com/) container and deployed to [Azure Container Instances](https://azure.microsoft.com/en-us/services/container-instances/) service. A more detailed walk-through can be found in this blog post at the following [link](http://luisquintanilla.me/2018/05/11/deploy-netml-docker-aci/)

## Dependencies

[Docker](https://docs.docker.com/install/linux/docker-ce/ubuntu/)  
[Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)  
[.NET Core 2.0](https://www.microsoft.com/net/download/linux)  
[Docker Hub Account](https://hub.docker.com/)

## Project Instructions

1. Build The Project
2. Create The Model
3. Test Web API Locally
4. Package Web API
5. Test Web API Docker Image Locally
6. Push Docker Image to Docker Hub
7. Deploy to Azure Container Instances
8. Test Deployed Application

## 1. Build The Solution

### Clone Repository

```bash
git clone https://github.com/lqdev/mlnetacidemo.git
```

### Build Solution

```bash
cd mlnetacidemo
dotnet restore
dotnet build
```

## 2. Create The Model

Before we start our API, we need to create our machine learning model. This is done via the `model` project.

```bash
dotnet run -p model/model.csproj
```

## 3. Test Web API Locally

The `model` project should have created a file called `model.zip` inside of it's directory. Copy `model.zip` file inside the `model` project directory to the `api` directory.

### Start The Web API
```bash
dotnet run -p api/api.csproj
```

### Send Test Request

Use POSTMAN or Insomnia REST Clients to send a `POST` request to `http://localhost:5000/api/predict` that includes the following body

```json
{
	"SepalLength": 3.3,
	"SepalWidth": 1.6,
	"PetalLength": 0.2,
	"PetalWidth": 5.1,
}
```

## 4. Package Web API

Using the `Docker CLI` tool, and your own Docker Hub username and image name of your choice, the following command will create a Docker image of the application using the `Dockerfile` in the root solution directory.

```bash
docker build -t <DOCKERUSERNAME>/<IMAGENAME>:latest .
```

## 5. Test Web API Docker Image Locally

### Start Docker Image

Using your DockerHub username and recently created image name from the last example, enter the following command to start the Docker image locally and bind it to port 5000.

```bash
docker run -d -p 5000:80 <DOCKERUSERNAME>/<IMAGENAME>:latest
```

### Send Test Request

Use POSTMAN or Insomnia REST Clients to send a `POST` request to `http://localhost:5000/api/predict` that includes the following body

```json
{
	"SepalLength": 3.3,
	"SepalWidth": 1.6,
	"PetalLength": 0.2,
	"PetalWidth": 5.1,
}
```

## 6. Push Docker Image to Docker Hub

To make our Docker image accessible to everyone else and the Azure Container Instance service, we need to push it to Docker Hub. This can be done with the following command

```bash
docker login
docker push <DOCKERUSERNAME>/<IMAGENAME>:latest
```

## 7. Deploy to Azure Container Instances

### Prepare Manifest File

The `azuredeploy.json` file helps define the deployment configurations for the application. However, in order to specify your Docker image as the one that will be deployed, the `containerimage` property needs to be replaced with your Docker Hub username and name of the image you just pushed to Docker Hub.

### Deploy Application

#### Login

```bash
az login
```

#### Create Resource Group

```bash
az group create --name mlnetacidemogroup --location eastus
```

#### Deploy

```bash
az group deployment create --resource-group mlnetacidemogroup --template-file azuredeploy.json
```

## 8. Test Deployed Application

Give it a few minutes for your deployment to initialize. If the deployment was successful, you should see some output on the command line. Look for the ContainerIPv4Address property. This is the IP Address where your container is accessible. In POSTMAN or Insomnia, replace the URL to which you previously made a `POST` request to with `http://<ContainerIPv4Address>/api/predict` where ContainerIPv4Address is the value that was returned to the command line after the deployment. If successful, the response should be just like previous requests Iris-virginica.

Once you’re finished, you can clean up resources with the following command:

```bash
az group delete --name mlnetacidemogroup
```