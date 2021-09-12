var express = require('express');
var router = express.Router();

const https = require('https')

const pageTitle = "Programmers' Week 2021 Container Demo"
var pageSubTitle = "Welcome to the Container Demo"
var displayMode = 'init'
var weatherData = ''

/* GET home page. */
router.get('/', function(req, res) {
  res.render('index', { title: pageTitle, pageData: {subTitle: pageSubTitle, displayMode: displayMode} });
});

router.post('/', function(req, res) {

  const options = {
    hostname: 'api.openweathermap.org',
    port: 443,
    path: '/data/2.5/weather?q={City}&units=imperial&appid=7d3f12acba15c51555254eba2f926324',
    method: 'GET',
    headers: {
      Accept: 'application/json'
    }
  }

  let data = '';

  options.path = options.path.replace('{City}', encodeURIComponent(req.body.city));

  const httpreq = https.get(options, resHttp => {
    console.log(`statusCode: ${resHttp.statusCode}`)
  
    resHttp.on('data', chunk => {
      data += chunk;
    })

    resHttp.on('end', () => {
      console.log(data);
      weatherData = JSON.parse(data)
      displayMode = 'results'
      pageSubTitle = 'Results here'
      res.render('index', { title: pageTitle, pageData: {subTitle: pageSubTitle, displayMode: displayMode, weatherData: weatherData} });
    })
  }).end();

  req.on('error', error => {
    console.error(error)
    res.render('error', {message: "this message", error: {status: "some status", stack: "this is the stack"}})
  })
})

module.exports = router;
