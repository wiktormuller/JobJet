import "./map-styles.css";
import React from "react";
import L from "leaflet";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import "leaflet/dist/leaflet.css";

export const Map = (props) => {
  let center = [52.006376, 19.025167];
  let zoom = 6.8;
  let skill = 0;

  const filteredLocalization = props.localizationArray.filter(
    (loc) => loc.id === 1
  );
  filteredLocalization.map(
    (loc) => ((center = [loc.lat, loc.lng]), (zoom = loc.zoom))
  );

  const filteredAdverts = props.adsArray.map(
    (ad) => (
      (skill = ad.skills[0]),
      (
        <Marker
          icon={L.icon({
            iconUrl: require(`../../data/icons/` + skill + `.svg`).default,
            iconSize: new L.Point(60, 75),
          })}
          key={ad.id}
          position={[ad.lat, ad.lng]}
        >
          <Popup position={[ad.lat, ad.lng]}>
            <div>
              <h2>{ad.title}</h2>
              <p>{ad.description}</p>
            </div>
          </Popup>
        </Marker>
      )
    )
  );

  return (
    <MapContainer
      className="map"
      center={center}
      zoom={zoom}
      scrollWheelZoom={true}
    >
      <TileLayer
        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
      {filteredAdverts}
    </MapContainer>
  );
};
