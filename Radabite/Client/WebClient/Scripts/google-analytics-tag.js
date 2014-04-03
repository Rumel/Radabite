//tracking
(function (i, s, o, g, r, a, m) {
	i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
		(i[r].q = i[r].q || []).push(arguments)
	}, i[r].l = 1 * new Date(); a = s.createElement(o),
	m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

//When this is on a public url:
//ga('create', 'UA-49653336-1', 'actual url');
ga('create', 'UA-49653336-1', { 'cookieDomain': 'none' });
ga('send', 'pageview');

//send an event when CreateEvent modal is opened
$('#CreateEventButton').on('click', function () {
	ga('send', 'pageview', {
		'page': 'CreateEvent',
		'title': 'Create Event modal'
	});
});