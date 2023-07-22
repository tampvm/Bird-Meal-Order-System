Feature: Login
	As a registered user
	I want to be able to login to my account
	So that I can access my account information

Scenario: Successful login
  Given I am on the login page
  When I enter valid login credentials
  And I click the login button
  Then I should be redirected to the home page

Scenario: Unsuccessful login
  Given I am on the login page
  When I enter invalid login credentials
  And I click the login button
  Then I should see an error message

