# API Documentation

This document provides detailed information about the BlockedCountry API endpoints, request/response formats, and usage examples.

## Table of Contents

1. [Countries API](#countries-api)
   - [Block Country](#block-country)
   - [Unblock Country](#unblock-country)
   - [Get Blocked Countries](#get-blocked-countries)
   - [Add Temporal Block](#add-temporal-block)
2. [IP API](#ip-api)
   - [IP Lookup](#ip-lookup)
   - [Check IP](#check-ip)
3. [Logs API](#logs-api)
   - [Get Logs](#get-logs)

## Countries API

### Block Country

Adds a country to the blocked list.

**Endpoint:** `POST /api/countries/block`

**Request Body:**

```json
{
  "countryCode": "RU",
  "countryName": "Russia",
  "blockDurationMinutes": 60
}
```

**Query Parameters:**

- `durationMinutes` (optional): Duration in minutes for the block to remain active. If not provided, the block is permanent.

**Response:**

- `200 OK`: Country successfully blocked
- `400 Bad Request`: Invalid request (missing country code)
- `409 Conflict`: Country already blocked

### Unblock Country

Removes a country from the blocked list.

**Endpoint:** `DELETE /api/countries/unblock/{code}`

**Path Parameters:**

- `code`: The two-letter country code to unblock.

**Response:**

- `200 OK`: Country successfully unblocked
- `404 Not Found`: Country not found in blocked list

### Get Blocked Countries

Retrieves a paginated list of blocked countries.

**Endpoint:** `GET /api/countries/blocked`

**Query Parameters:**

- `page` (default: 1): The page number
- `pageSize` (default: 10): The number of items per page
- `search` (optional): Filter by country code or name

**Response:**

```json
{
  "items": [
    {
      "countryCode": "RU",
      "countryName": "Russia",
      "unblockAt": "2025-06-01T12:00:00Z"
    },
    {
      "countryCode": "CN",
      "countryName": "China",
      "unblockAt": null
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 2
}
```

### Add Temporal Block

Adds a temporary block for a country.

**Endpoint:** `POST /api/countries/block/temporal`

**Request Body:**

```json
{
  "countryCode": "RU",
  "durationMinutes": 60
}
```

**Response:**

- `200 OK`: Country successfully blocked temporarily
- `400 Bad Request`: Invalid request
- `409 Conflict`: Country already blocked

## IP API

### IP Lookup

Gets the country for an IP address.

**Endpoint:** `GET /api/ip/ip-lookup`

**Query Parameters:**

- `ipAddress` (optional): The IP address to lookup. If not provided, uses the requester's IP.

**Response:**

```json
{
  "ipAddress": "8.8.8.8",
  "countryCode": "US",
  "countryName": "United States"
}
```

### Check IP

Checks if an IP address is from a blocked country.

**Endpoint:** `POST /api/ip/check`

**Request Body:**

```json
{
  "ipAddress": "8.8.8.8"
}
```

**Response:**

```json
{
  "ipAddress": "8.8.8.8",
  "countryCode": "US",
  "countryName": "United States",
  "isBlocked": false
}
```

## Logs API

### Get Logs

Retrieves logs of access attempts.

**Endpoint:** `GET /api/logs`

**Query Parameters:**

- None

**Response:**

```json
[
  {
    "ipAddress": "178.22.33.44",
    "timestamp": "2025-05-30T10:15:30Z",
    "countryCode": "RU",
    "countryName": "Russia",
    "blockedStatus": true
  },
  {
    "ipAddress": "8.8.8.8",
    "timestamp": "2025-05-30T10:16:45Z",
    "countryCode": "US",
    "countryName": "United States",
    "blockedStatus": false
  }
]
```
