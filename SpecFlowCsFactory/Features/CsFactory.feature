Feature: CsFactoryFeature
Simple calculator for adding two numbers

    Scenario: Create default Model
        Given Create User name is "Yuan"
        When Query user name is "Yuan"
        Then Name is "Yuan" number is "1"

    Scenario: Create 2 default Model
        Given Create User name is "Yuan"
        And Create User name is "Ame"
        When Query user name is "Ame"
        Then Name is "Ame" number is "2"

    Scenario: Create 3 default Model
        Given Create User name is "Yuan"
        And Create User name is "Ame"
        And Create User name is "Dika"
        When Query user name is "Dika"
        Then Name is "Dika" number is "10"

    Scenario: Create Order that has User
        Given Create User name is "Yuan"
        And Create Order id is "123456"
        When Query order id is "123456"
        Then User name is "Yuan"

    Scenario: Test DeepCopy is work
        Given Create user name is "Roy" number is "10" to be actual
        And Fork user name is "Roy"  be fork
        When actual number is "-99"
        Then expected user number is "10"

    Scenario: Test ToExpected is work
        Given Create user name is "Roy" number is "10" to be actual
        And Fork user name is "Roy"  be expected and set Password is "asdf1234"
        When actual number is "-99"
        Then expected user number is "10"
        And expected user password is "asdf1234"