import { useState, useEffect } from "react";
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';
import "../../styles/form-styles.css";
import { getTeams } from "../../modules/teamManager";
import { getSiteTime } from "../../modules/siteTimeManager";
import { postGame } from "../../modules/gameManager";

export default function GameForm() {
    const [teams, setTeams] = useState([]);
    const [game, setGame] = useState({});

    useEffect(() => {
        getTeams().then((teams) => {
            setTeams(teams);
        }).then(
            getSiteTime().then((siteTime) => {
                const date = new Date();
                const dateStr = `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;

                setGame(
                    {
                        homeTeamId: 0,
                        awayTeamId: 0,
                        kickoffTime: `${dateStr}T12:00`,
                        week: siteTime.currentWeek,
                        year: siteTime.currentYear
                    }
                )
            }
        ));
    }, []);

    return (
        <Form className="form-container" style={{width: "50%", marginTop: "1%"}}>
            <FormGroup>
                <Label for="awayTeam">Away Team</Label>
                <Input type="select" name="awayTeam" id="away-team" value={game.awayTeamId} onChange={(e) => {
                    const copy = {...game}
                    copy.awayTeamId = parseInt(e.target.value);
                    setGame(copy);
                }}>
                    <option id="team--0">Select a Team...</option>
                    {
                        teams.map((team) => {
                            return <option 
                                key={`team--${team.id}`}
                                value={team.id}
                            >{team.fullName}</option>
                        })
                    }
                </Input>
            </FormGroup>
            <FormGroup>
                <Label for="homeTeam">Home Team</Label>
                <Input type="select" name="homeTeam" id="home-team" value={game.homeTeamId} onChange={(e) => {
                    const copy = {...game}
                    copy.homeTeamId = parseInt(e.target.value);
                    setGame(copy);
                }}>
                    <option id="team--0" value={0}>Select a Team...</option>
                    {
                        teams.map((team) => {
                            return <option 
                                key={`team--${team.id}`} 
                                value={team.id}
                            >{team.fullName}</option>
                        })
                    }
                </Input>
            </FormGroup>
            <FormGroup>
                <Label for="exampleEmail">Kickoff Time</Label>
                <Input 
                    type="datetime-local" 
                    name="kickoffTime" 
                    id="kickoff-time" 
                    value={game.kickoffTime}
                    onChange={(e) => {
                        const copy = {...game};
                        copy.kickoffTime = e.target.value;
                        setGame(copy);
                    }}
                /> 
            </FormGroup>
            <FormGroup>
                <Label for="currentWeek">Current Week</Label>
                <Input 
                    type="number" 
                    name="currentWeek" 
                    id="current-week" 
                    defaultValue={game.week}
                    onChange={(e) => {
                        const copy = {...game};
                        copy.week = parseInt(e.target.value);
                        setGame(copy);
                    }}
                /> 
            </FormGroup>
            <FormGroup >
                <Label for="currentYear">Current Year</Label>
                <Input 
                    type="number" 
                    name="currentYear" 
                    id="current-year" 
                    defaultValue={game.year}
                    onChange={(e) => {
                        const copy = {...game};
                        copy.year = parseInt(e.target.value);
                        setGame(copy);
                    }} 
                /> 
            </FormGroup>
            <Button onClick={() => {
                console.log(game)
                if (game.homeTeamId === 0 || game.awayTeamId === 0) {
                    window.alert("Error: Must select a home and away team");
                    return;
                } else if (game.homeTeamId === game.awayTeamId) {
                    window.alert("Error: The same team cannot be selected for both the home and away team");
                    return;
                }

                postGame(game).then(() => {
                    const copy = {...game};
                    copy.awayTeamId = 0;
                    copy.homeTeamId = 0;
                    setGame(copy);
                });
            }}>Submit</Button>
        </Form>
    );
}