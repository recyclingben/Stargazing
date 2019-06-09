import React, { Component } from "react";
import { ClickCountDisplay } from "./ClickCountDisplay";
import { Star } from "./Star";
import { Spacer, SPACER_HEIGHT } from "../Spacer/Spacer";
import { ScreenSize } from "../ScreenSize/ScreenSize";
import { delay, randomBetween, nil } from "../../util/util";

import styles from "./StarPanel.module.css";

export class StarPanel extends Component {
    constructor(props) {
        super(props);

        this.state = {
            starCount: 0,
            personalStarCount: localStorage.getItem("personal_star_count") || 0,
            id: Math.random()
        };
        this.absorbtion = 0;

        this.setup = this.setup.bind(this);
        this.addStars = this.addStars.bind(this);
        this.slowAddStars = this.addStarsGradually.bind(this);
        this.handleSRStarIncremented = this.handleSRStarIncremented.bind(this);
        this.button_addStars = this.button_addStars.bind(this);

        this.setup();
    }

    componentDidMount() {
        this.props.starUpdatesSR.onConnectionReady(connection => {
            connection.on("StarIncremented", this.handleSRStarIncremented);
        });
    }

    async componentWillUnmount() {
        this.props.starUpdatesSR.onConnectionReady(async connection => {
            connection.off("StarIncremented", this.handleSRStarIncremented);
        });
    }

    async setup() {
        const starCountResponse = await fetch("http://www.api.stargazing.io/stars/count");
        const starCountJson = await starCountResponse.json();
        const starCount = starCountJson.data.count;

        const estimatedFrequencyResponse = await fetch("http://www.api.stargazing.io/stars/est-frequency");
        const estimatedFrequencyJson = await estimatedFrequencyResponse.json();
        const estimatedFrequency = estimatedFrequencyJson.data.frequency;

        this.addStarsGradually(starCount - estimatedFrequency, false, 1);

        this.addStarsGradually(estimatedFrequency);
    }

    /* Immediately adds stars to `this.state.starCount`. Returns false if all stars were absorbed. */
    async addStars(amount, absorb = true) {
        if (amount >= this.absorbtion || !absorb) {
            let toAdd = amount;
            if (absorb) {
                toAdd = amount - this.absorbtion;
                this.absorbtion = 0;
            }

            await this.setState({
                starCount: this.state.starCount + toAdd
            });

            return true;
        } else {
            this.absorbtion = this.absorbtion - amount;
            return false;
        }
    }

    async addStarsGradually(amount, absorb = true, seconds = 15) {
        let groups = amount > 100 ? 100 : amount;
        /* Double tilde means fast `Math.max()` for positive numbers. */
        let incrementation = ~~(amount / groups);


        const averageDelay = seconds / groups;
        let offset = 0;
        let previousOffset;

        for (let i = 0; i < groups+1; i++) {
            await delay(averageDelay + offset - previousOffset);

            previousOffset = offset;
            offset = randomBetween(-averageDelay * 0.5, averageDelay * 0.5);

            const add = i !== groups - 1
                ? incrementation
                : amount % incrementation;
            this.addStars(add, absorb)
                .then(notAllAbsorbed => {
                    if (notAllAbsorbed && !nil(this.starButton))
                        this.starButton.wiggle();
                });
        }
    }

    async handleSRStarIncremented(data) {
        this.addStarsGradually(data.amount);
    }

    async button_addStars() {
        fetch("http://www.api.stargazing.io/stars/increment", { method: "POST" });

        ++this.absorbtion;
        this.state.personalStarCount = localStorage.getItem("personal_star_count");
        localStorage.setItem("personal_star_count", ++this.state.personalStarCount);

        await this.addStars(1, false);
    }

    render() {
        return (
            <div className={styles.Container}>
                <div className={styles.StarPanel}>
                    <ScreenSize max={1000}>
                        <Spacer height={SPACER_HEIGHT.large} />
                    </ScreenSize>

                    <ClickCountDisplay total={this.state.starCount} personal={this.state.personalStarCount} />

                    <ScreenSize max={1000}>
                        <Spacer height={SPACER_HEIGHT.large} />
                    </ScreenSize>

                    <Star onClick={this.button_addStars} ref={star => this.starButton = star} />
                </div>
            </div>
        );
    }
}
