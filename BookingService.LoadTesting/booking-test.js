import http from "k6/http";
import { check, sleep } from "k6";

export let options = {
  vus: 5,           
  duration: "10s",  
};

export default function () {
  const url = "https://localhost:7018/api/booking/create";
  const payload = JSON.stringify({
    bookingTime: "09:30",
    name: `Test User`
  });

  const params = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  let res = http.post(url, payload, params);

  console.log(`VU ${__VU} -> Status: ${res.status}, Body: ${res.body}`);

  check(res, {
    "status is 200": (r) => r.status === 200,
    "status is 400": (r) => r.status === 400,
    "status is 409": (r) => r.status === 409,
  });

  sleep(1); 
}
