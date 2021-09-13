/*
  YOU MUST PROVIDE YOUR OWN VALUE HERE 
  Obtain your own key by signing up at:
  https://openweathermap.org/
*/
const weatherApiAppId = '<YOUR-API-ID-HERE>';

var express = require('express');
var router = express.Router();
const https = require('https')

const pageTitle = "Programmers' Week 2021 Container Demo"
var weatherData = ''

/* GET */
router.get('/', function(req, res) {
  res.render('index', { title: pageTitle, pageData: {displayMode: 'init'} });
});

/* POST */
router.post('/', function(req, res) {

  const options = {
    hostname: 'api.openweathermap.org',
    port: 443,
    path: '/data/2.5/weather?q={City}&units=imperial&appid=' + weatherApiAppId,
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
      
      if (resHttp.statusCode != 404) {
        weatherData = JSON.parse(data)
        res.render('index', { title: pageTitle, pageData: {displayMode: 'results', weatherData: weatherData} });
      } else {
        res.render('index', { title: pageTitle, pageData: {displayMode: 'notfound'} });
      }
    })
  }).end();

  req.on('error', error => {
    console.error(error)
    res.render('error', {message: error.message, error: {status: error.status, stack: error.stack}})
  })
})

module.exports = router;
