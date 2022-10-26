import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';
import "../../styles/form-styles.css";
import { getGameById } from "../../modules/gameManager";
import { postBet, getLiveBetForGame, closeBet } from "../../modules/betManager";


export default function BetPropertiesForm() {
    const [game, setGame] = useState({});
    const [bet, setBet] = useState({});
    const {gameId} = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        getGameById(gameId).then((game) => {
            getLiveBetForGame(gameId).then((bet) => {
                if (bet) {
                    setBet({
                        id: bet.id,
                        gameId: game.id,
                        betTypeId: 1,
                        line: null,
                        awayTeamOdds: bet.awayTeamOdds,
                        homeTeamOdds: bet.homeTeamOdds,
                    });
                } else {
                    setBet({
                        id: 0,
                        gameId: game.id,
                        betTypeId: 1,
                        line: null,
                        awayTeamOdds: null,
                        homeTeamOdds: null,
                    });
                }
            });

            setGame(game);
        });
    }, []);

    return (
        <Form className="form-container" style={{width: "50%"}}>
            <FormGroup>
            <Label for="awayTeamOdds">{game?.awayTeam?.fullName} Odds</Label>
                <Input 
                    type="number" 
                    name="awayTeamOdds"
                    defaultValue={bet.awayTeamOdds}
                    onChange={(e) => {
                        const copy = {...bet}
                        copy.awayTeamOdds = parseInt(e.target.value);
                        setBet(copy);
                    }}
                />
            </FormGroup>
            <FormGroup>
            <Label for="homeTeamOdds">{game?.homeTeam?.fullName} Odds</Label>
                <Input 
                    type="number" 
                    name="homeOddsScore"
                    defaultValue={bet.homeTeamOdds}
                    onChange={(e) => {
                        const copy = {...bet}
                        copy.homeTeamOdds = parseInt(e.target.value);
                        setBet(copy);
                    }}
                />
            </FormGroup>
            {
                bet.id 
                ?
                    <div className="button-row">
                        <Button 
                            onClick={() => {
                                postBet(bet).then(() => {navigate("/")});
                            }}
                        >Adjust Odds</Button>
                        <Button
                            onClick={() => {
                                closeBet(bet).then(() => {navigate("/")});
                            }}
                        >Close Bets For Game</Button>
                    </div>
                : 
                    <Button 
                        onClick={() => {
                            postBet(bet).then(() => {navigate("/")});
                        }}
                    >Open Bet</Button> 
            }
        </Form>
    );
}