import { check, sleep } from "k6";
import http from "k6/http";

export const options = {
  vus: 10,
  iterations: 10,
};

export default function () {
  const params = {
    cookies: {},
    headers: { "x-api-version": "1.0" },
  };

  const res = http.get(
    "http://localhost:5096/api/v1/weatherforecast",
    params
  );

  check(res, {
    "is status 200": (r) => r.status === 200,
    "is status 429": (r) => r.status === 429,
  });

  sleep(1);
}