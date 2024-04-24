import React from "react";
import "./styles.css";
import IMAGE from "./react.png";

export const App = () => {
  return (
    <>
      <h1>
        Test Home Page - {process.env.NODE_ENV} {process.env.name}
      </h1>
      <img src={IMAGE} alt="React Logo" width="300" height="300" />
    </>
  );
};
