import React from "react";
import PlayerCard from "./player-card";

export default function PlayerCardList({cards}) {
    return (
        <> 
        {cards?.map( card => <PlayerCard key={card.Nome + Math.floor(Math.random() * 1000) + card.Classe + Math.floor(Math.random() * 1000)} attributes={card} /> )}
        </>
    )
}
