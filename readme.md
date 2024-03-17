## Request Code

```
https://login.microsoftonline.com/9e8754b6-f9cd-4aed-974d-a0ec0f3ed703/oauth2/v2.0/authorize?
response_type=code&
client_id=55317597-9ae0-4183-96f3-1bd60ca8fe4c&
redirect_uri=http://localhost:5000/redirecturi&
scope=openid
```

## Exchange Code for Token

```
https://login.microsoftonline.com/9e8754b6-f9cd-4aed-974d-a0ec0f3ed703/oauth2/v2.0/token
```

## Postman request to trade code for token

![Postman post request](./tradeCodeForAccessToken.png)
