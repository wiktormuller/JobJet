import "../styles/main-styles.css";
// import { useState /*, useEffect*/ } from "react";
// import { Routes, Route } from "react-router-dom";
import { Header } from "./header/Header.jsx";
import { Data } from "./data/Data.jsx";
// import { AdvertDetails } from "./advertDetails/AdvertDetails.jsx";
import { Map } from "./map/Map.jsx";

export const LandingPage = (props) => {
  return (
    <div className="main-container">
      <Header
        userLogInState={props.userLogInState}
        setUserLogInState={props.setUserLogInState}
      ></Header>
      <div className="sub-container">
        {/* <Routes> */}
        {/* <Route path="/" element={<Data />} /> */}
        {/* </Routes> */}

        <Data
          localizationArray={props.localizationArray}
          skillsArray={props.skillsArray}
          adsArray={props.adsArray}
          searchedSkills={props.searchedSkills}
          setSearchedInput={props.setSearchedInput}
          setSearchedLocalization={props.setSearchedLocalization}
          setSearchedSkills={props.setSearchedSkills}
        ></Data>
        {/* <AdvertDetails /> */}
        <Map
          localizationArray={props.localizationArray}
          adsArray={props.adsArray}
        ></Map>
      </div>
    </div>
  );
};
