library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity EDLatch_testbench is
--  Port ( );
end EDLatch_testbench;

architecture Behavioral of EDLatch_testbench is
    Component EDLatch is
    Port ( E : in STD_LOGIC;
           D : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);    
    end component;
    
    Component EDLatch_beh is
    Port ( E : in STD_LOGIC;
           D : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
   end component;
   
   function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;
   
   signal e, d, sq, snq, bq, bnq: std_logic := '0';

begin

    b : EDLatch_beh port map(E=>e, D=>d, Q=>bq, NQ=>bnq);
    s : EDLatch port map(E=>e, D=>d, Q=>sq, NQ=>snq);

    Main: process
    begin
        report "Testbench started!";

        e <= '1';
        d <= '0';
        wait for 10 ns;
        
        for i in 0 to 1 loop 
            for j in 0 to 1 loop 
                e <= to_stdlogic(i);
                d <= to_stdlogic(j);
                wait for 10 ns;
                assert ((sq /= bq) or (snq /= bnq)) 
                    report "Testbench failed! at " & integer'image(i) & " " 
                    & integer'image(j);
            end loop;
        end loop;
        report "Testbench finished";
        wait;
    end process;

end Behavioral;
