import http from "k6/http";
import { check, sleep } from "k6";

export let options = {
  vus: 5,           
  duration: "10s",  
};


function getName() {
  const letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  const suffix = Array.from({ length: 3 }, () =>
    letters[Math.floor(Math.random() * letters.length)]
  ).join("");
  return `User ${suffix}`;
}

export default function () {
  const url = "https://localhost:7018/api/booking/create";
  const payload = JSON.stringify({
    bookingTime: "09:30",
    name: getName()
  });

  const params = {
    headers: {
      "Content-Type": "application/json",
    },
  };

  let res = http.post(url, payload, params);

  console.log(`Name: ${JSON.parse(payload).name} -> Status: ${res.status}, Body: ${res.body}`);

  check(res, {
    "status is 200": (r) => r.status === 200,
    "status is 400": (r) => r.status === 400,
    "status is 409": (r) => r.status === 409,
  });

  sleep(1); 
}
