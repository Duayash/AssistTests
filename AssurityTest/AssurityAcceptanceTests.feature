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

@unit
Scenario Outline: Assert Category parameters from the response
Then Name is <Name>, CanRelist is <RFlag> and Promotion element <PromoName> has a description containing text <Text>

Examples:
    | Name           | RFlag | PromoName | Text				   |
    | Carbon credits | True  | Gallery   | 2x larger image     |
