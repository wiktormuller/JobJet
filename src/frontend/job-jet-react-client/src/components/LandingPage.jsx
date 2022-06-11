import "../styles/main-styles.css";
import { useState /*, useEffect*/ } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Header } from "./header/Header.jsx";
import { Data } from "./data/Data.jsx";
import { AdvertDetails } from "./advertDetails/AdvertDetails.jsx";
import { Map } from "./map/Map.jsx";
import { RegistrationPanel } from "./registrationPanel/RegistrationPanel";
import { LoginPanel } from "./loginPanel/LoginPanel";
import { DenialPage } from "./denialPage/DenialPage";
import { AddJobOfferPanel } from "./addJobOfferPanel/AddJobOfferPanel";
//import { AddCompany } from "./addCompany/AddCompany";

export const LandingPage = (props) => {
  const [advertDetails, setAdvertDetails] = useState({});
  const [geoLocation, setGeoLocation] = useState({});
  const [advertLocation, setAdvertLocation] = useState({});
  return (
    <div className="main-container">
      <Header
        userLogInState={props.userLogInState}
        setUserLogInState={props.setUserLogInState}
        setGeoLocation={setGeoLocation}
        setAdvertLocation={setAdvertLocation}
      ></Header>
      <div className="sub-container">
        <Routes>
          <Route
            path="/"
            element={
              <Data
                localizationArray={props.localizationArray}
                skillsArray={props.skillsArray}
                jobOffersArray={props.jobOffersArray}
                searchedSkills={props.searchedSkills}
                setSearchedInput={props.setSearchedInput}
                setSearchedLocalization={props.setSearchedLocalization}
                setSearchedSkills={props.setSearchedSkills}
                setAdvertDetails={setAdvertDetails}
                setAdvertLocation={setAdvertLocation}
              />
            }
          />
          <Route
            path="/details"
            element={
              <AdvertDetails
                advertDetails={advertDetails}
                setGeoLocation={setGeoLocation}
              />
            }
          />
          <Route
            path="/register"
            element={
              <RegistrationPanel
                userLogInState={props.userLogInState}
                setUserLogInState={props.setUserLogInState}
              />
            }
          />
          <Route
            path="/login"
            element={
              <LoginPanel
                setUserLogInState={props.setUserLogInState}
                setToken={props.setToken}
              />
            }
          />
          <Route path="/denial" element={<DenialPage />} />
          <Route
            path="/add-job-offer"
            element={
              <AddJobOfferPanel
                token={props.token}
                setToken={props.setToken}
                countries={props.localizationArray}
                companies={props.companies}
                skills={props.skillsArray}
                currencies={props.currencies}
                seniority={props.seniority}
                employmentTypes={props.employmentType}
              />
            }
          />
        </Routes>

        <Map
          localizationArray={props.localizationArray}
          jobOffersArray={props.jobOffersArray}
          geoLocation={geoLocation}
          advertLocation={advertLocation}
        />
      </div>
    </div>
  );
};
