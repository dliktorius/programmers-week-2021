# Programmers' Week 2021
Repository: programmers-week-2021

This repository contains code for demos from my "Cloud Native Apps" presentation at Cognizant Softvision Programmers' Week 2021.

## Getting Started
You must have the following downloaded and installed to build and run these demos:

| Prerequisite | AWS Lambda Demo | Container Demo |
| ----------- | ----------- | ----------- |
| Supported Platform(s) | Local or AWS | Local, AWS or Azure |
| [Visual Studio 2019 (or later)](https://visualstudio.microsoft.com/)<br>- ASP.NET and web development workload | Yes<br>Required | - |
| [.NET SDK Version](https://dotnet.microsoft.com/download/) | 3.1 Core | - |
| [AWS Toolkit for Visual Studio](https://aws.amazon.com/visualstudio/) | Yes | - |
| [Postman](https://www.postman.com/downloads/) | Optional | - |
| [Visual Studio Code](https://code.visualstudio.com/) | - | Yes |
| [Docker Desktop](https://www.docker.com/products/docker-desktop) may also require [WSL 2 Linux kernel](https://aka.ms/wsl2kernel) | - | Yes |
| [VS Code Docker Extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker) | - | Yes |
| [Node.js 10 (or later)](https://nodejs.org/) | - | Yes |
| [PowerShell 7.1.4 (or later)(https://github.com/PowerShell/PowerShell/releases/latest) | - | Optional |
| The code from this repository | Yes | Yes |

The code in this repository can be run locally or deployed to Amazon AWS and/or Microsoft Azure, respectively.

**Note: Deploying resources within Amazon AWS or Microsoft Azure can and will incur costs you will be liable for.**

## Demos
### AWS Lambda Demo
The **AWSLambdaDemo** project makes use of [AWS Lambda](https://aws.amazon.com/lambda/) to host a serverless Microsoft .NET Core based Web API project.
The API has two endpoints; both accept an image file to be processed by the [AWS Rekognition Image](https://aws.amazon.com/rekognition/) service. The Lambda function is fronted by [Amazon API Gateway](https://aws.amazon.com/api-gateway/) and deployed via CloudFormation using the *serverless.template* file.

**Requirements:** HTTP POST containing multipart/form-data. The image must be submitted in the form field named `file`.
- **/api/labels/** - This endpoint uses the label detection feature of Rekognition to output a list of label(s) or tag(s) on whether the image is an object, scene, action, or concept.
- **/api/celebrities/** - This endpoint uses the celebrity recognition feature of Rekognition to automatically recognize tens of thousands of well-known personalities in the image using machine learning.

### Container Demo
The **ContainerDemo** project makes use of Docker to containerize a Node.js based Web API project.

You may wish to review the [Quick Start Guide](https://code.visualstudio.com/docs/containers/quickstart-node) this demo was created from.

To deploy the demo resources using the provided Azure ARM template, you can deploy them ysing the Azure Portal or with the included PowerShell script.  Note: PowprerShell v7.1 or later (7.1.4 was used for this demo) should be used and is the preferred major release of PowerShell recommended by Microsoft for Azure.
