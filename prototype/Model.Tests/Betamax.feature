Feature: Betamax
	In order to test my application
	As a test writer
	I want to be able to record my api calls and have them play back in order

@OneCall
Scenario: Record one api call and playback
	Given I clear tape 1
	Given I record api call 1 on tape 1
	When I playback tape 1
	Then api call 1 should replay

@TwoCall
Scenario: Record two api calls and playback in order
    Given I clear tape 1
	Given I clear tape 2
	Given I record api call 1 on tape 2
	Given I record api call 2 on tape 2
	When I playback tape 2
	Then api call 1 should replay
	When I playback tape 2
	Then api call 2 should replay


