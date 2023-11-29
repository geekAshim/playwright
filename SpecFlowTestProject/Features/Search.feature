Feature: Search

@mytag
Scenario: Can find search results
  Given I am on "https://www.google.com"
  When I search for "dotnetcoretutorials.com"
  Then I should see result "https://dotnetcoretutorials.com"
