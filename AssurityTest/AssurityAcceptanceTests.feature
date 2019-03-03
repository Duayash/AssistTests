Feature: AssurityAcceptanceTests
	
Background: 
Given A request is made to the service and response is saved

| Url |
| https://api.tmsandbox.co.nz/v1/Categories/6327/Details.json?catalogue=false | 

	
@unit
Scenario Outline: Assert Category service returns a 200 code
Then The service status code should be <ResponseCode>

 Examples:
    | ResponseCode |
    | 200          |

@acceptance
Scenario Outline: Assert Category name from the response
Then Name is <Name>

Examples:
    | Name           | 
    | Carbon credits | 

@acceptance
Scenario Outline: Assert Category CanRelist from the response
Then CanRelist is <RFlag> 

Examples: 
    | RFlag |
    | True  |

@acceptance
Scenario Outline: Assert Promotion Name with description from the response
Then Promotion element is <PromoName> has a description containing text <Text>

Examples:
    | PromoName | Text				   |
    | Gallery   | 2x larger image      |


