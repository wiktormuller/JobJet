import "./loginPanel-styles.css";
import { useState, useEffect } from "react";
import { Link } from "react-router-dom";

export const LoginPanel = (props) => {
  let [email, setEmail] = useState("");
  let [password, setPassword] = useState("");

  const authLogin = `https://jobjet.azurewebsites.net/api/v1/auth/login`;

  const handleClick = () => {
    let body = {
      email: `${email}`,
      password: `${password}`,
    };

    let strBody = JSON.stringify(body);

    fetch(authLogin, {
      method: "POST",
      headers: {
        Accept: "application/json, text/plain",
        "Content-Type": "application/json;charset=UTF-8",
      },
      body: strBody,
    })
      .then((response) => response.json())
      .then((data) => {
        if (!!data.accessToken) {
          props.setToken(data.accessToken);
          props.setUserLogInState(true);
        } else {
          alert(data);
        }
      });
  };

  return (
      <form className="form-login">
        <h2>Zaloguj się</h2>
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
        <Link className="custom-link" to="/">
          <button type="button-click" onClick={handleClick}>
            Zaloguj
          </button>
        </Link>
      </form>
  );
};
