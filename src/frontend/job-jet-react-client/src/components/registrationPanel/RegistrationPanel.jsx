import "./registrationPanel-styles.css";
import { useState, useEffect } from "react";

export const RegistrationPanel = (props) => {
  let [name, setName] = useState("");
  let [email, setEmail] = useState("");
  let [password, setPassword] = useState("");

  const registerLogin = `https://jobjet.azurewebsites.net/api/v1/auth/register`;

  const handleClick = () => {
    let body = {
      name: `${name}`,
      email: `${email}`,
      password: `${password}`,
    };

    let strBody = JSON.stringify(body);

    fetch(registerLogin, {
      method: "POST",
      headers: {
        Accept: "application/json, text/plain",
        "Content-Type": "application/json;charset=UTF-8",
      },
      body: strBody,
    })
      .then((response) => response.json())
      .then((data) => {
        console.log(data);
      });
  };

  return (
      <form className="form-register">
        <h2>Zarejestruj się</h2>
        <input
          className="custom-input"
          input
          type="text"
          placeholder="Enter Name"
          name="name"
          id="name"
          required
          onChange={(e) => {
            setName(e.target.value);
          }}
        ></input>
        <br />
        <input
          className="custom-input"
          input
          type="text"
          placeholder="Enter Email"
          name="email"
          id="email"
          required
          onChange={(e) => {
            setEmail(e.target.value);
          }}
        ></input>
        <br />
        <input
          className="custom-input"
          type="password"
          placeholder="Enter Password"
          name="psw"
          id="psw"
          required
          onChange={(e) => {
            setPassword(e.target.value);
          }}
        ></input>
        <br />
        <button type="button-click" onClick={handleClick}>
          Zarejestuj
        </button>
      </form>
  );
};
