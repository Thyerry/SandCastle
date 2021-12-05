import React from "react";
import PlayerCard from "./player-card";

export default function PlayerCardList({cards}) {
    return (
        <> 
        {cards?.map( card => <PlayerCard attributes={card} /> )}
        </>
    )
}
