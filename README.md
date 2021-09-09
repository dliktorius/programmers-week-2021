# Programmers' Week 2021
Repository: programmers-week-2021

This repository contains code for demos from my "Cloud Native Apps" presentation at Cognizant Softvision Programmers' Week 2021.

## Getting Started
You must have the following downloaded and installed to run these demos:
- [Microsoft Visual Studio 2019 (or later)](https://visualstudio.microsoft.com/) with the **ASP.NET and web development** workload installed
- [AWS Toolkit for Visual Studio](https://aws.amazon.com/visualstudio/) for the AWS Demos
- [Postman](https://www.postman.com/downloads/)
- The code from this repository

The code in this repository can be run locally or deployed to Amazon AWS and Microsoft Azure, respectively.

**Note: Deploying resources within Amazon AWS or Microsoft Azure can and will incur costs you will be liable for.**

## Demos
### AWS Lambda Demo
The **AWSLambdaDemo** project makes use of [AWS Lambda](https://aws.amazon.com/lambda/) to host a serverless Microsoft .NET Core based Web API project.
The API has two endpoints; both accept an image file to be processed by the [AWS Rekognition Image](https://aws.amazon.com/rekognition/) service. The Lambda function is fronted by [Amazon API Gateway](https://aws.amazon.com/api-gateway/) and deployed via CloudFormation using the *serverless.template* file.

**Requirements:** HTTP POST containing multipart/form-data. The image must be submitted in the form field named `file`.
- **/api/labels/** - This endpoint uses the label detection feature of Rekognition to output a list of label(s) or tag(s) on whether the image is an object, scene, action, or concept.
- **/api/celebrities/** - This endpoint uses the celebrity recognition feature of Rekognition to automatically recognize tens of thousands of well-known personalities in the image using machine learning.
