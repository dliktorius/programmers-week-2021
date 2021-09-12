var express = require('express');
var router = express.Router();

const https = require('https')



const options = {
  hostname: 'api.openweathermap.org',
  port: 443,
  path: '/data/2.5/weather?q={City}&appid=7d3f12acba15c51555254eba2f926324',
  method: 'GET',
  headers: {
    Accept: 'application/json'
  }
}


/* GET users listing. */
router.get('/', function(req, res, next) {

  let data = '';

  options.path = options.path.replace('{City}', encodeURIComponent('New York'));

  const httpreq = https.get(options, resHttp => {
    console.log(`statusCode: ${resHttp.statusCode}`)
  
    resHttp.on('data', chunk => {
      data += chunk;
    })

    resHttp.on('end', () => {
      console.log(data);
      //res.send(data);
      res.render('index', { title: "Weather" });
    })
  }).end();

  req.on('error', error => {
    console.error(error)
  })


});

module.exports = router;

