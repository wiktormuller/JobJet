import React from "react";

import { useState } from "react";
import '../styles/data-styles.css';

import * as DiIcons from "react-icons/di";
import * as BiIcons from "react-icons/bi";
import * as SiIcons from "react-icons/si";
import * as MdIcons from "react-icons/md";
import * as RiIcons from "react-icons/ri";
import * as BsIcons from "react-icons/bs";

const nameMapping = {
  Di: DiIcons,
  Bi: BiIcons,
  Si: SiIcons,
  Md: MdIcons,
  Ri: RiIcons,
  Bs: BsIcons
}

const Icon = props => {
  const { iconName, size, color } = props;

  const Icon = nameMapping[iconName.slice(0, 2)][iconName]
  
  return <div style={{ fontSize: size, color: color }}><Icon /></div>;
};


export const FilterButton = (props) => {
    const [active, setActive] = useState(true);
    const [clicked, setClicked] = useState("");

    const handleClick = () => {
      setActive(!active);
      if (active === true) {
        setClicked("--clicked");
        setActive(false);
        props.setSearchedSkills(old => [...old, props.id])
      }
      else {
        setClicked("");
        props.setSearchedSkills(old => old.filter(id => id !== props.id))
      }
    };
  
    return (
      <button
        className={`data__filter-data--btn${clicked}`}
        onClick={handleClick}
      >
        <div className="data__filter-data--btn--icon">
          <Icon iconName={props.icon} size={'2em'} color={props.color}/>
          <span>{props.name}</span>
        </div>
      </button>
    );
  };