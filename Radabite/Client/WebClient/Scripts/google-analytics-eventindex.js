//Google Analytics:
//send an pageview when EditEvent modal is opened
$('#EditEventButton').on('click', function () {
	ga('send', 'pageview', {
		'page': 'EditEvent',
		'title': 'Edit Event modal'
	});
});

$('#DeleteEventButton').on('click', function () {
	ga('send', 'pageview', {
		'page': 'DeleteEvent',
		'title': 'Delete Event modal opened'
	});
});

$('#UpdateSubmitButton').on('click', function () {
	ga('send', {
		'hitType': 'event',
		'eventCategory': 'Event updated',
		'eventAction': 'click',
	});
});

$('#DeleteSubmitButton').on('click', function () {
	ga('send', {
		'hitType': 'event',
		'eventCategory': 'Event deleted',
		'eventAction': 'click',
	});
});

$('#PostSubmitButton').on('click', function () {

	ga('send', {
		'hitType': 'event',
		'eventCategory': 'Posted to Event @Model.Id',
		'eventAction': 'click',
	});
});

$(function () {
	ga('send', {
		'hitType': 'event',
		'eventCategory': 'Viewed event page @Model.Id',
		'eventAction': 'view',
		'eventValue': parseInt('@Model.Id'),
		'eventLabel': '@Model.Id'
	});
});
