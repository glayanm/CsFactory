Feature: CsFactoryFeature
Simple calculator for adding two numbers

    @mytag
    Scenario: Create default Model
        When Create default Model
        Then Name is "Name#1" number is "1"

    Scenario: Create 2 default Model
        When Create default Model
        When Create default Model
        Then Name is "Name#2" number is "2"