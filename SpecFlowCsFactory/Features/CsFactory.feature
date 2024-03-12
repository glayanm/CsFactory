Feature: CsFactoryFeature
Simple calculator for adding two numbers

    Scenario: Create default Model
        When Query user name is "Yuan"
        Then Name is "Yuan" number is "1"

    Scenario: Create 2 default Model
        When Query user name is "Ame"
        Then Name is "Ame" number is "2"

    Scenario: Create 3 default Model
        When Query user name is "Dika"
        Then Name is "Dika" number is "10"
        
    Scenario: Create Order that has User
        When Query order id is "123456"
        Then User name is "Yuan"