import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';
import "../../styles/form-styles.css";
import { getGameById } from "../../modules/gameManager";
import { postBet } from "../../modules/betManager";


export default function BetPropertiesForm() {
    const [game, setGame] = useState({});
    const [bet, setBet] = useState({});
    const {gameId} = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        getGameById(gameId).then((game) => {
            setBet({
                gameId: game.id,
                betTypeId: 1,
                line: null,
                awayTeamOdds: null,
                homeTeamOdds: null,
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
            <Label for="homeTeamOdds">{game?.homeTeam?.fullName} Score</Label>
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
            <Button onClick={() => {
                postBet(bet).then(() => {navigate("/")})
            }}>Open Bet</Button>
        </Form>
    );
}