Feature: AssurityAcceptanceTests
	
Background: 
Given A request is made to the service

| Url |
| https://api.tmsandbox.co.nz/v1/Categories/6327/Details.json?catalogue=false | 
When The response has been saved

	
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
