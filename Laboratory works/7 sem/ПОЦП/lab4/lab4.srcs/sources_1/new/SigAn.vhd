----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 07:09:10 PM
-- Design Name: 
-- Module Name: SigAn - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity SigAn is
    Generic (pol: std_logic_vector := "1100001");
    Port ( Clk : in STD_LOGIC;
           Din : in STD_LOGIC;
           Rst : in STD_LOGIC;
           Dout : out STD_LOGIC_VECTOR (0 to pol'high-1));
end SigAn;

architecture Behavioral of SigAn is
    signal sreg, sdat : std_logic_vector(0 to pol'high-1);
begin
    m: process(Clk, Rst)
        variable c : std_logic;
    begin
        if Rst = '1' then
            sreg <= (others => '0');
            sdat <= (others => '0');
        elsif rising_edge(Clk) then
            c := sreg(sreg'high-1);
            for i in pol'high-2 downto 0 loop
                if pol(i) = '1' then
                    sdat(i+1) <= sreg(i) xor c;   
                else
                    sdat(i+1) <= sreg(i);
                end if;
            end loop;
            sdat(0) <= Din xor c;
            sreg <= sdat;
        end if;
    end process;
    
    Dout <= sreg;

end Behavioral;
