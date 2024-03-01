Feature: CsFactory
	Simple calculator for adding two numbers

@mytag
Scenario: Get Model Default Value
	When CsFactory create a model
	Then the model name name is "name" , number is "0"
	