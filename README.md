# SfQuery

.Net-based command-line utility to query Salesforce data.

This is mostly a proof of concept on how to integrate with Salesforce's REST API, but it still can be used to query data in the org.

## Introduction

SFQuery uses the Salesforce REST API to perform SOQL queries based the data in your organisation.

## Setup

You will need to create a Connected App endpoint in your org for SFQuery to be able to log onto it.

You will then need to configure your credentials (username/password/connected app ID and secret) in the App.config file. 

Step-by-step information to be found [here](https://blog.mkorman.uk/integrating-net-and-salesforce-part-1-rest-api/).

## Usage


### Querying the data in your org

To run a query:

`SfQuery <query>`

For instance:

`SfQuery "Select ID, Name from Contact"`
