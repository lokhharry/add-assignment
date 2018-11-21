import Vue from 'vue'
import Vuetify from 'vuetify'
import axios from 'axios'

Vue.use(Vuetify)

new Vue({
    el: '#app',
    data() {
        return {
            districts: [],
            selectedDistrict: null,
            estates: [],
            selectedEstate: null,
            propertys: [],
            propertyTableHeader: [
                { text: 'District', sortable: false, value: 'DistrictName' },
                { text: 'Estate', sortable: false, value: 'EstateName' },
                { text: 'Block', sortable: false, value: 'Block' },
                { text: 'Floor', sortable: false, value: 'Floor' },
                { text: 'Flat', sortable: false, value: 'Flat' },
                { text: 'Gross Floor Area', sortable: false, value: 'GrossFloorArea' },
                { text: 'No of Bedroom', sortable: false, value: 'NumberOfBedroom' },
                { text: 'Provide Car Park', sortable: false, value: 'CarParkProvided' },
                { text: 'Selling Price', sortable: false, value: 'SellingPrice' },
                { text: 'Rental Price', sortable: false, value: 'RentalPrice' }
            ],
        }
    },
    methods: {
        getEstate: function () {
            let self = this
            axios({
                method: 'get',
                url: '/Home/getEstate',
                responseType: 'json',
                params: {
                    districtID: this.selectedDistrict
                }
            }).then(function (response) {
                self.estates = response.data
            })
        },
        getProperty: function () {
            let self = this
            axios({
                method: 'get',
                url: '/Home/getProperty',
                responseType: 'json',
                params: {
                    estateID: this.selectedEstate
                }
            }).then(function (response) {
                self.propertys = response.data
            })
        }
    },
    watch: {
        //selectedDistrict: function () {
        //    this.getEstate()
        //},
        //selectedEstate: function () {
        //    this.getProperty()
        //}
    },
    mounted: function () {
        this.districts = viewmodel_listdistrict
    }
})