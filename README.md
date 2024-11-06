# Computer Vision API 

## Instruktionerna för körning av applikation i lokalmiljö:
1.	Klona repo:t 
```bash
git clone https://github.com/Himoazo/ComputerVision.git
```
2.	Gå till appsettings.json:
```json
"JWT": {
    "Issuer": "",
    "Audience": "",
    "Signingkey": ""
  }
  ```
3. Fyll i de tomma parametrarna ovan med följande data:
```json
"JWT": {
    "Issuer": "http://localhost:5258/",
    "Audience": "http://localhost:5258/",
    "Signingkey": "a99da746a436244a52123da05f6be0bdaa4c964809f0d3815aa0dcccb93fdcb0f54d69908634892d6004c5d263a5ed30dd5dd234baad58691566d2ad0afeab6a"
  }
  ```
*Obs.* Issuer och Audience värde ska matcha med sökvägen i den lokala miljön om porten 5258 är upptagen då ska 5258 ändras till den aktuella port som applikationen körs på.
*Obs.* Signingkey måste uppfylla speciella kriterier nyckeln ovan uppfyller dessa krav men en annan nyckel kan också användas.


4.	Build och run. 
5.	Swagger gränssnitt rekommenderas att lokalt testa tjänsten med alternativt klona projektet som konsumerar tjänsten och kör den lokalt men glöm inte att ändra URL i den till din lokala miljö.
API base url: ” https://computervision-production.up.railway.app/”
Denna webbtjänst konsumeras via denna [webbplats]( https://filters-dt071g.netlify.app/) koden för webbplatsen finns [här](https://github.com/Himoazo/filters)


## API Endpoints

### /api/account/login

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### /api/account/register

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### /api/images

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

#### GET
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### /api/images/{id}

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

#### PUT
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |
