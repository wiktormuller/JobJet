import '../styles/data-styles.css';
import { AdvertSynopsis } from "./AdvertSynopsis"

export const AdvertData = (props) => {

    const renderedArray = props.adsArray.map(
      (ad) => {
        return <AdvertSynopsis 
            key={ad.id} 
            title={ad.title}
            localization={ad.localization}
            salary={ad.salary}
            date={ad.date}
            description={ad.description}
            skills={ad.skills}
            skillsArray={props.skillsArray}
            localizationArray={props.localizationArray}
            >
            </AdvertSynopsis>
      }
    )

    return (
      <div className="data__advert-data">
        {renderedArray}
      </div>
      );
}